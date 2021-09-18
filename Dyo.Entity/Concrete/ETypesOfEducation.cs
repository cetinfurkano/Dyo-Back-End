using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Dyo.Entity.Concrete
{
    public enum ETypesOfEducation
    {
        [Description("Lise")]
        HighSchool = 1,
        [Description("İlkokul")]
        PrimarySchool = 2,
        [Description("Ortaokul")]
        MiddleSchool = 3,
        [Description("Okul Öncesi")]
        PreSchool = 4,
        NonType = -1

    }
}
