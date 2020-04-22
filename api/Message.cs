using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace api
{
    
    public class Message
    {
        public byte[] Payload {get;set;}
        public string DataPath {get;set;}
        public JObject Metadata {get; set;}
    }
}