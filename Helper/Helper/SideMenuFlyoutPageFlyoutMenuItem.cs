using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Helper
{
    public class SideMenuFlyoutPageFlyoutMenuItem
    {
        public SideMenuFlyoutPageFlyoutMenuItem()
        {
            TargetType = typeof(SideMenuFlyoutPageFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }

        public string icon { get; set; }
    }
}