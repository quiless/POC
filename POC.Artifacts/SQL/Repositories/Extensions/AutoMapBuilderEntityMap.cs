using System;
using System.Linq.Expressions;
using System.Reflection;
using Dapper.FluentMap.Dommel.Mapping;
using POC.Artifacts.SQL.Repositories.Builders;
using System.Linq.Dynamic.Core;

namespace POC.Artifacts.SQL.Repositories.Extensions
{
    public class AutoMapBuilderEntityMap<TEntity> : DommelEntityMap<TEntity> where TEntity : class
    {
        /// <summary>
        /// Realiza o mapeamento da entidade no Dommel.
        /// </summary>
        public AutoMapBuilderEntityMap()
        {
            Type TType = typeof(TEntity);

            var pTypeAttributes = (UseAutoMapBuilder[])TType.GetCustomAttributes(typeof(UseAutoMapBuilder), false);

            if (pTypeAttributes == null || pTypeAttributes.Length == 0)
                return;

            if (pTypeAttributes.Length > 0 && pTypeAttributes[0].DisableFluentMap)
                return;

            if (!string.IsNullOrEmpty(pTypeAttributes[0].SchemaName))
                ToTable($"{pTypeAttributes[0].SchemaName}.{TType.Name}");
            else
                ToTable(TType.Name.ToLower());

            PropertyInfo[] Properties = TType.GetProperties();
            foreach (PropertyInfo property in Properties)
            {
                var pAttributes = (AutoMapBuilder[])property.GetCustomAttributes(typeof(AutoMapBuilder), false);
                if (pAttributes.Length > 0)
                {
                    var map = Map(ParseLambda<TEntity, object>($"x=>x.{property.Name}"));

                    if (pAttributes[0].typesBuilder
                        .Any(x => x == AutoMapBuilder.Type.Ignore))
                        map.Ignore();

                    if (pAttributes[0].typesBuilder
                        .Any(x => x == AutoMapBuilder.Type.Identity))
                        map.IsIdentity().IsKey();

                    if (pAttributes[0].typesBuilder
                        .Any(x => x == AutoMapBuilder.Type.Key))
                        map.IsKey();

                    if (pAttributes[0].typesBuilder
                        .Any(x => x == AutoMapBuilder.Type.Computed))
                        map.SetGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);

                    if (!string.IsNullOrEmpty(pAttributes[0].columnName))
                        map.ToColumn(pAttributes[0].columnName.ToLower());
                }
                else if (!property.GetType().IsValueType)
                {
                    
                    var map = Map(ParseLambda<TEntity, object>($"x=>x.{property.Name}"));

                    if (property.Name == "Id")
                        map.IsIdentity().IsKey();

                    map.ToColumn(property.Name.ToLower());
                }
            }
        }

        /// <summary>
        /// Analisa e monta a expressão lambda.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="expression">Expressão a ser analizada.</param>
        /// <returns>Retorna delegate de <see cref="Expression"/>.</returns>
        public static Expression<Func<T, S>> ParseLambda<T, S>(string expression)
        {
            string paramString = expression[..expression.IndexOf("=>")].Trim();
            string lambdaString = expression[(expression.IndexOf("=>") + 2)..].Trim();
            ParameterExpression param = Expression.Parameter(typeof(T), paramString);

            return (Expression<Func<T, S>>)DynamicExpressionParser.ParseLambda(new[] { param }, typeof(S), lambdaString, null);
        }

        public static LambdaExpression ParseLambda(string expression, Type returnType, params Type[] paramTypes)
        {
            string paramString = expression[..expression.IndexOf("=>")].Trim("() ".ToCharArray());
            string lambdaString = expression[(expression.IndexOf("=>") + 2)..].Trim();

            var paramList = paramString.Split(',');
            if (paramList.Length != paramTypes.Length)
                throw new ArgumentException("Specified number of lambda parameters do not match the number of parameter types!", "expression");

            List<ParameterExpression> parameters = new List<ParameterExpression>();
            for (int i = 0; i < paramList.Length; i++)
                parameters.Add(Expression.Parameter(paramTypes[i], paramList[i].Trim()));

            return DynamicExpressionParser.ParseLambda(parameters.ToArray(), returnType, lambdaString, null);
        }
    }
}

