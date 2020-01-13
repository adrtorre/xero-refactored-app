using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XeroRefactoredApp.DTOs
{
    interface IDoDtoConverter<M, D>
    {
        D FromDO(M model);

        M ToDO(D dto);
    }
}
