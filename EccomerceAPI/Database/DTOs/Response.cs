using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DTOs
{
    public class Response<T>
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public string[] Errors { get; set; }

        public override string ToString() => $"Status: {Status}, Message: {Message}, Data: {Data} Errors: {string.Join("\n", Errors)}";
    }
}
