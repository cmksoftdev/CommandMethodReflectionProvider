using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CMK.Services;

namespace WpfApp
{
    public class ViewModel
    {
        public CommandMethodReflectionProvider MyProperty { get; }
        public int Test;

        public ViewModel()
        {
            Test = 293;
            this.MyProperty = new CommandMethodReflectionProvider(this);
        }

        private void method(float a, char b, ref int c)
        {
            c++;
            MessageBox.Show("Button wurde gedrückt! :)");
        }

        public void Method2()
        {
            MessageBox.Show("Button2 wurde gedrückt! :D");
        }

        public void Method3()
        {
            MessageBox.Show("Button3 wurde gedrückt! xD");
        }
    }
}
