using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace check_yo_self_api.Server.Entities
{
    public class GettingStarted
    {
        [Key]
        [JsonIgnore]
        public string _id {get; set;}
        public string ApplicationName { get; set; }
    }
}