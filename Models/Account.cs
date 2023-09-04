using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP_BE_Interview.Models
{
    public class SimplifiedAccount
    {
        public const string CLASS_NAME = "Account";
        public const string CONTAINER_NAME = "Account";

        public string ClassName { get; set; } = CLASS_NAME;
        public string accountName { get; set; }
        public string Status { get; set; }
    }
}
