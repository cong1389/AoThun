using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Infrastructure.DependencyManagement
{
    public class Work<T> where T : class
    {
        private readonly Func<Work<T>, T> _resolve;

        public Work(Func<Work<T>, T> resolve)
        {
            _resolve = resolve;
        }

        public T Value
        {
            get { return _resolve(this); }
        }
    }
}
