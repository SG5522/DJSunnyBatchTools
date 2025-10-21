using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public partial class LockStoreCleaner : Component
    {
        public LockStoreCleaner()
        {
            InitializeComponent();
        }

        public LockStoreCleaner(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
