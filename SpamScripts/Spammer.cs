namespace Transit.SpamScripts;

public class Spammer
{
    /*
     *
     *  string localAddr = "http://127.0.0.1:5022";
            await Task.Delay(3000);
            var data = "USER_ID=User-12321&ANI=Ani&DNIS=Dnis&EVENT=Event&VO_ID=25&PARAMS=Params&CTI_EVENT=52";
            
            var result = await $"{localAddr}/cisco/newmsg?{data}".GetAsync();
     */
    // builder.WebHost.UseUrls("http://*:51943"); // to build custom port
    // 10.3.1.15 - удаленный доступ
    /*
     *  var itemsVM = options.Select(o =>
                    {
                        var ctiType = CtiType.Other;
                        var ctiTypeStr = string.Empty;
                        if (o.Selectors.FirstOrDefault(x => x.Name == "cti_type")?.Attributes.TryGetValue("value", out ctiTypeStr) ?? false)                       
                            Enum.TryParse(ctiTypeStr, true, out ctiType);

                        var packType = (int)CtiPackageType.Base;
                        var packTypeStr = string.Empty;
                        if ((o.Selectors.FirstOrDefault(x => x.Name == "pack_type")?.Attributes.TryGetValue("value", out packTypeStr) ?? false))                        
                            int.TryParse(packTypeStr, out packType);

                        var connectedServiceItems = serviceItems.Where(s => s.CtiType == ctiType);

                        if (connectedServiceItems?.Any() ?? false) o.IsEnabled = true;

                        return new CtiItemViewModel 
                        {
                            Option = o,
                            FeatureModel = CtiFeatureService.GetModel(ctiType),
                            Domains = connectedServiceItems?.Select(x => x.CrmDomain).ToArray(),
                            PackType = (CtiPackageType)packType, 
                            Type = ctiType
                        };
                    });
     */
}