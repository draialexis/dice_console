using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Games;

namespace Data
{
    public interface ILoader
    {
        public GameRunner LoadApp();
    }
}
