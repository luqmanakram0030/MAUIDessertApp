using System;
using System.Collections.Generic;
using System.Text;
namespace Desserts.Domain.Services.Interface
{
    public interface IPrintService
    {
        byte[] Print(string content);
    }
}
