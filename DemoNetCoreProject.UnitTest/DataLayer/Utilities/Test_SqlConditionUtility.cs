using Dapper;
using DemoNetCoreProject.DataLayer.Enums;
using DemoNetCoreProject.DataLayer.Utilities;
using System.Collections;
using System.Data;
using System.Text;

namespace DemoNetCoreProject.UnitTest.DataLayer.Utilities
{
    [TestClass]
    public class Test_SqlConditionUtility
    {
        [TestMethod]
        public void Run()
        {
            var sql = new StringBuilder(" SELECT * FROM [TABLE] ");
            var dynamicParameters = new DynamicParameters();
            SqlConditionUtility.Add(sql, "[COLUMN] = Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "[COLUMN]", SqlOperatorType.Equal, "Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "[COLUMN]", SqlOperatorType.NotEqual, "Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "[COLUMN]", SqlOperatorType.GreaterThan, "Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "[COLUMN]", SqlOperatorType.GreaterThanOrEqual, "Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "[COLUMN]", SqlOperatorType.LessThan, "Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "[COLUMN]", SqlOperatorType.LessThanOrEqual, "Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "[COLUMN]", SqlOperatorType.LikeStart, "Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "[COLUMN]", SqlOperatorType.LikeEnd, "Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "[COLUMN]", SqlOperatorType.LikeContain, "Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "[COLUMN]", SqlOperatorType.NotLikeStart, "Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "[COLUMN]", SqlOperatorType.NotLikeEnd, "Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "[COLUMN]", SqlOperatorType.NotLikeContain, "Value");
            SqlConditionUtility.Add(sql, dynamicParameters, "[COLUMN]", SqlOperatorType.Equal, "1");
            SqlConditionUtility.Add(sql, dynamicParameters, "[COLUMN]", SqlOperatorType.Equal, DateTime.Now.AddHours(1));
            SqlConditionUtility.Add(sql, dynamicParameters, "[COLUMN]", SqlOperatorType.Equal, Convert.ToInt32("1"));
            SqlConditionUtility.Add(sql, dynamicParameters, "[COLUMN]", SqlOperatorType.Equal, Convert.ToDouble("1"));
            SqlConditionUtility.Add(sql, dynamicParameters, "[COLUMN]", SqlOperatorType.Equal, true, DbType.Boolean);
            SqlConditionUtility.Add(sql, dynamicParameters, "[COLUMN]", SqlOperatorType.Contain, new List<string>() { "1", "2" });
            SqlConditionUtility.Add(sql, dynamicParameters, "[COLUMN]", SqlOperatorType.NotContain, new List<string>() { "1", "2" });
            SqlConditionUtility.Add(sql, dynamicParameters, "[COLUMN]", SqlOperatorType.Contain, new List<string>() { "1", "2" });
            SqlConditionUtility.Add(sql, dynamicParameters, "[COLUMN]", SqlOperatorType.Contain, new List<DateTime>() { DateTime.Now.AddHours(1), DateTime.Now.AddHours(2) });
            SqlConditionUtility.Add(sql, dynamicParameters, "[COLUMN]", SqlOperatorType.Contain, new List<int>() { Convert.ToInt32("1"), Convert.ToInt32("2") });
            SqlConditionUtility.Add(sql, dynamicParameters, "[COLUMN]", SqlOperatorType.Contain, new List<long>() { Convert.ToInt64("1"), Convert.ToInt64("2") });
            SqlConditionUtility.Add(sql, dynamicParameters, "[COLUMN]", SqlOperatorType.Contain, new List<double>() { Convert.ToDouble("1"), Convert.ToInt64("2") });

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
                        Console.WriteLine($"[{parameterName}]:[{stringBuilder}],");
                    }
                    else
                    {
                        Console.WriteLine($"[{parameterName}]:[{dynamicParameter}],");
                    }
                }
                else
                {
                    Console.WriteLine($"[{parameterName}]:[NULL],");
                }
            });
        }
    }
}