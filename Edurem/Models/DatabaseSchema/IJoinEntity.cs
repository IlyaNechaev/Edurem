using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Models
{
    public interface IJoinEntity<TFirstEntity, TSecondEntity>
    {
        TFirstEntity FirstEntity { get; set; }
        TSecondEntity SecondEntity { get; set; }
    }
}
