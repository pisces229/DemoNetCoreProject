using Dapper;
using DemoNetCoreProject.Common.Enums;
using DemoNetCoreProject.Common.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Data;
using System.Text;

namespace DemoNetCoreProject.UnitTest.Domain.Utilities
{
    [TestClass]
    public class UnitTest_SqlConditionUtility
    {
        [TestMethod]
        public void Run()
        {
            var sql = new StringBuilder();
            var dynamicParameters = new DynamicParameters();
            SqlConditionUtility.Add(sql, "Text = Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "Text", SqlOperatorType.Equal, "Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "Text", SqlOperatorType.NotEqual, "Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "Text", SqlOperatorType.GreaterThan, "Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "Text", SqlOperatorType.GreaterThanOrEqual, "Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "Text", SqlOperatorType.LessThan, "Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "Text", SqlOperatorType.LessThanOrEqual, "Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "Text", SqlOperatorType.LikeStart, "Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "Text", SqlOperatorType.LikeEnd, "Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "Text", SqlOperatorType.LikeContain, "Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "Text", SqlOperatorType.NotLikeStart, "Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "Text", SqlOperatorType.NotLikeEnd, "Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "Text", SqlOperatorType.NotLikeContain, "Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "Text", SqlOperatorType.Equal, "1");
            SqlConditionUtility.Add(sql, dynamicParameters, "Text", SqlOperatorType.Equal, DateTime.Now.AddHours(1));
            SqlConditionUtility.Add(sql, dynamicParameters, "Text", SqlOperatorType.Equal, Convert.ToInt32("1"));
            SqlConditionUtility.Add(sql, dynamicParameters, "Text", SqlOperatorType.Equal, Convert.ToDouble("1"));
            SqlConditionUtility.Add(sql, dynamicParameters, "Text", SqlOperatorType.Equal, true, DbType.Boolean);
            SqlConditionUtility.Add(sql, dynamicParameters, "Text", SqlOperatorType.Contain, new List<string>() { "1", "2" });
            SqlConditionUtility.Add(sql, dynamicParameters, "Text", SqlOperatorType.NotContain, new List<string>() { "1", "2" });
            SqlConditionUtility.Add(sql, dynamicParameters, "Text", SqlOperatorType.Contain, new List<string>() { "1", "2" });
            SqlConditionUtility.Add(sql, dynamicParameters, "Text", SqlOperatorType.Contain, new List<DateTime>() { DateTime.Now.AddHours(1), DateTime.Now.AddHours(2) });
            SqlConditionUtility.Add(sql, dynamicParameters, "Text", SqlOperatorType.Contain, new List<int>() { Convert.ToInt32("1"), Convert.ToInt32("2") });
            SqlConditionUtility.Add(sql, dynamicParameters, "Text", SqlOperatorType.Contain, new List<long>() { Convert.ToInt64("1"), Convert.ToInt64("2") });
            SqlConditionUtility.Add(sql, dynamicParameters, "Text", SqlOperatorType.Contain, new List<double>() { Convert.ToDouble("1"), Convert.ToInt64("2") });

            Console.WriteLine(sql.Replace("AND", $"{Environment.NewLine}AND").ToString());
            dynamicParameters.ParameterNames.ToList().ForEach(parameterName =>
            {
                var dynamicParameter = dynamicParameters.Get<object>(parameterName);
                if (dynamicParameter != null)
                {
                    var dynamicParameterType = dynamicParameter.GetType();
                    if (dynamicParameter is IList)
                    {
                        var stringBuilder = new StringBuilder();
                        foreach (var value in (dynamicParameter as IList)!)
                        {
                            if (stringBuilder.Length > 0)
                            {
                                stringBuilder.Append(',');
                            }
                            stringBuilder.Append(value != null ? value.ToString() : "null");
                        }
                        Console.WriteLine($"[{ parameterName }]:[{ stringBuilder }],");
                    }
                    else
                    {
                        Console.WriteLine($"[{ parameterName }]:[{ dynamicParameter }],");
                    }
                }
                else
                {
                    Console.WriteLine($"[{ parameterName }]:[NULL],");
                }
            });
        }
    }
}