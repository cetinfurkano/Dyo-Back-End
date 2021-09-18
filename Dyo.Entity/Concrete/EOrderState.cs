using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Dyo.Entity.Concrete
{
    public enum EOrderState : byte
    {
        [Description("Bekliyor")]
        Waiting = 0,
        [Description("Tamamlandı")]
        Completed = 1,
        [Description("Devam ediyor")]
        Continues = 2,
        [Description("İptal edildi")]
        Cancelled = 3,
        [Description("Kargoya verildi")]
        Shipped = 4,
        [Description("Kabul edildi")]
        Accepted = 5,

    }
}
