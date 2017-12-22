using App.Core.Caching;
using App.Service.LocaleStringResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Common
{
    public class CommonServices : ICommonServices
    {
        private readonly Lazy<IWorkContext> _workContext;

        private readonly Lazy<ILocaleStringResourceService> _localization;

        private readonly Lazy<ICacheManager> _cacheManager;

        public CommonServices(Lazy<IWorkContext> workContext, Lazy<ILocaleStringResourceService> localization, Lazy<ICacheManager> cacheManager)
        {
            this._workContext = workContext;
            this._localization = localization;
            _cacheManager = cacheManager;
        }

        public IWorkContext WorkContext
        {
            get
            {
                return _workContext.Value;
            }

        }

        public ILocaleStringResourceService Localization
        {
            get
            {
                return _localization.Value;
            }
        }

        public ICacheManager Cache
        {
            get
            {
                return _cacheManager.Value;
            }
        }
    }
}
