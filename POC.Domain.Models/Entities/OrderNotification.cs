using System;
using POC.Artifacts.Domain.Models;
using POC.Artifacts.SQL.Repositories.Builders;

namespace POC.Domain.Models.Entities
{
	[UseAutoMapBuilder("ordernotification",false)]
	public class OrderNotification: Entity<Int32>
	{
		public int OrderId { get; set; }
		public int MotorcycleRentalId { get; set; }
		public DateTime CreateDate { get; set; }

        /* 
		 * Não deu tempo de implementar 
		 * 
		 * A ideia era conseguir gravar o momento que a notificação foi recebida no dispositivo do entregador, e o momento que ele clicou.
		 * 
		 * Considerando que seria implementando PushNotification, poderiamos mensurar quanto tempo o entregador demora para abrir a notificação, e mensurar disponibilidade individual de cada um.
		 * 
		 * 
		 * 
		 * 
		 * 
		 */

        //public bool WasClicked { get; set; }
        //public DateTime? ClickedDate { get; set; }
        //public bool WasReceived { get; set; }
        //public DateTime? ReceivedDate { get; set; }

    }
}

