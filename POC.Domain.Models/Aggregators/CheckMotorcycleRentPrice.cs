using System;
using POC.Domain.Models.Enumerations;

namespace POC.Domain.Models.Aggregators
{
	public class CheckMotorcycleRentPrice
	{
		public bool HasFine { get; set; }
		public decimal TotalValue { get; set; }
		public decimal TotalFineValue { get; set; } 
		public DateTime ExpectedEndDate { get; set; }
		public DateTime StartDate { get; set; }
		public int TotalDaysPlan { get; set; }
		public decimal DailyPrice { get; set; }
		public decimal AdditionalDailyPrice { get; set; }
		public DateTime ReturnDate { get;  set; }
		public decimal PercentageFine { get; set; }
		public int AdditionalDays { get; set; }
		public int AdvanceDays { get; set; }
		public RentFinishTypeEnum RentFinishType { get; set; }



		public CheckMotorcycleRentPrice(
			DateTime startDate,
			DateTime expectedEndDate,
			DateTime returnDate,
			int totalDaysPlan,
			decimal dailyprice,
			decimal additionalDailyPrice,
			decimal percentageFine)
		{
			this.StartDate = startDate;
			this.ExpectedEndDate = expectedEndDate;
			this.ReturnDate = returnDate;
			this.TotalDaysPlan = totalDaysPlan;
			this.DailyPrice = dailyprice;
			this.AdditionalDailyPrice = additionalDailyPrice;
			this.PercentageFine = percentageFine;

			this.RentFinishType = RentFinishTypeEnum.InDeadline;


			//A locação inicia-se no dia subsequente a contratação. Caso o entregar tente devolver a moto no mesmo dia que realizou a contratação, considerei o período integral do plano para aplicar a multa.
			if (ReturnDate < StartDate)
				ReturnDate = StartDate;

			//Verificar a data da solicitação x data prevista
			//As horas foram ignoradas para cálculo. Apenas dias.

			if (ReturnDate.Date == ExpectedEndDate.Date)
				this.TotalValue = this.DailyPrice * this.TotalDaysPlan;
			else if (ReturnDate.Date >= ExpectedEndDate)
				_calculateLateFee();
			else
				_calculateAdvanceFine();
        }


		private void _calculateAdvanceFine()
		{
			this.HasFine = true;

            this.RentFinishType = RentFinishTypeEnum.Advance;

            //Calcular dias de a atencedência = Data de expectativa - Data de retorno = Dias de antecedência
            this.AdvanceDays = (int)(ExpectedEndDate.Date - ReturnDate.Date).TotalDays;

            //Calcular valor da taxa de antecipação
            this.TotalFineValue = (this.AdvanceDays * this.DailyPrice) * (this.PercentageFine/100);

			this.TotalValue = (this.TotalDaysPlan * this.DailyPrice) + this.TotalFineValue;
        }

		private void _calculateLateFee()
		{
			this.HasFine = true;

            this.RentFinishType = RentFinishTypeEnum.Late;

            //Calcular dias de atraso = Data de retorno - Data de expectativa = Dias de atraso
            this.AdditionalDays = (int)(ReturnDate.Date - ExpectedEndDate.Date).TotalDays;

            //Calcular valor da taxa de atraso = Data de retorno - Data de expectativa = Dias de atraso * Valor dia adicional
            this.TotalFineValue = this.AdditionalDays * this.AdditionalDailyPrice;

			//Calcular valor do plano + valor de atraso
			this.TotalValue = (this.DailyPrice * this.TotalDaysPlan) + this.TotalFineValue;
        }
    }



}

