using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductAPI.Models
{
    public class UpdateCountRequestModel
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
    }
}
