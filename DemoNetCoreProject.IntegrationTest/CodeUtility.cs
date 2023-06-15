using DemoNetCoreProject.Backend.Models.Default;
using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace DemoNetCoreProject.IntegrationTest
{
    [TestClass]
    public class CodeUtility
    {
        [TestMethod]
        public void ModelToType()
        {
            var model = new DefaultPageQueryBindInputModel();
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var json = JsonConvert.SerializeObject(model, jsonSerializerSettings);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            var result = new StringBuilder();
            result.AppendLine($"type {model.GetType().Name} = {{");
            foreach (var key in dictionary.Keys)
            {
                result.AppendLine($"{key}?: string;");
            }
            result.AppendLine("}");
            Console.WriteLine(result.ToString());
        }
        [TestMethod]
        public void InputDtoToBindInputModel()
        {
            var dto = new DefaultLogicPageQueryInputDto();
            var names = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                JsonConvert.SerializeObject(dto, new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }));
            var members = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                JsonConvert.SerializeObject(dto, new JsonSerializerSettings()));
            var result = new StringBuilder();
            for (var i = 0; i < names.Count(); ++i)
            {
                var name = names.Keys.Skip(i).First();
                var member = members.Keys.Skip(i).First();
                result.AppendLine($"[BindProperty(Name = \"{name}\")]");
                result.AppendLine($"[Required(ErrorMessage = $\"【{name}】{{ValidationErrorMessageConstant.Required}}\")]");
                result.AppendLine($"public string {member} {{ get; set; }} = null!;");
            }
            Console.WriteLine(result.ToString());
        }
        [TestMethod]
        public void InputDtoToJsonInputModel()
        {
            var dto = new DefaultLogicPageQueryInputDto();
            var names = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                JsonConvert.SerializeObject(dto, new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }));
            var members = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                JsonConvert.SerializeObject(dto, new JsonSerializerSettings()));
            var result = new StringBuilder();
            for (var i = 0; i < names.Count(); ++i)
            {
                var name = names.Keys.Skip(i).First();
                var member = members.Keys.Skip(i).First();
                result.AppendLine($"[JsonPropertyName(name:  \"{name}\")]");
                result.AppendLine($"[Required(ErrorMessage = $\"【{name}】{{ValidationErrorMessageConstant.Required}}\")]");
                result.AppendLine($"public string {member} {{ get; set; }} = null!;");
            }
            Console.WriteLine(result.ToString());
        }
    }
}
