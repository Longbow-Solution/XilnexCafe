using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CFSSK.API
{
    public class ApiFunc
    {
        private string _TraceCategory = "ApiFunc";

        public bool GetMenu(out ApiModel.GetMenu.Response res)
        {
            bool isTrue = false;
            res = null;

            try
            {
                ApiRequest apiRequest = new ApiRequest();

                string apiUrl = GeneralVar.ApiURL + "/v1/menumanager/menus?locationid="+GeneralVar.LocationID+"&menuProfileId="+ GeneralVar.MenuID +"&source="+ GeneralVar.Source;
                
                isTrue = apiRequest.SendGetRequest(apiUrl, null, GeneralVar.AppID,GeneralVar.token,GeneralVar.auth, out string responseBody);

                Trace.WriteLineIf(GeneralVar.SwcTraceLevel.TraceInfo, String.Format("[Info] GetMenu API Response = {0}\n", responseBody), _TraceCategory);

                //Trace.WriteLineIf(GeneralVar.SwcTraceLevel.TraceInfo, String.Format("[Info] GetCategory API Response = {0}\n", responseBody), _TraceCategory);

                if (isTrue)
                {
                    res = JsonConvert.DeserializeObject<ApiModel.GetMenu.Response>(responseBody);
                }

            }
            catch (Exception ex)
            {
                Trace.WriteLineIf(GeneralVar.SwcTraceLevel.TraceError, String.Format("[Error] GetCategory = {0}", ex.Message), _TraceCategory);
            }
            return isTrue;
        }
    }
}

