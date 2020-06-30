using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace memo2.Models
{
    /// <summary>
    /// 各プラットフォームごとのServiceの変更のためのインターフェイスです。
    /// </summary>
    public interface ICreateService
    {
        void ServiceOn();
        void ServiceOff();
    }
}
