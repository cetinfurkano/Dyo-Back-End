using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dyo.WebAPI.HelperDtos
{
    public class UpdateOrderModel
    {
        public string Id { get; set; }
        public int OrderState { get; set; }
        public bool IsValid { get; set; }
    }
}
