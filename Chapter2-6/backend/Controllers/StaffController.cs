using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ApplicationInsights;



namespace backend.Controllers
{
    [Route("api/[controller]")]
    public class StaffController : Controller
    {

        private readonly IReliableStateManager stateManager ;

        private TelemetryClient telemetry = new TelemetryClient();

        private object StaffDictionary;

        public StaffController(IReliableStateManager stateManager)
        {
            this.stateManager = stateManager  ;
        }


        // GET api/staff
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            CancellationToken ct = new CancellationToken();

            IReliableDictionary<string, int> StaffDictionary = await this.stateManager.GetOrAddAsync<IReliableDictionary<string, int>>("counts");

            using (ITransaction tx = this.stateManager.CreateTransaction())
            {
                Microsoft.ServiceFabric.Data.IAsyncEnumerable<KeyValuePair<string, int>> list = await StaffDictionary.CreateEnumerableAsync(tx);

                Microsoft.ServiceFabric.Data.IAsyncEnumerator<KeyValuePair<string, int>> enumerator = list.GetAsyncEnumerator();

                List<KeyValuePair<string, int>> result = new List<KeyValuePair<string, int>>();

                while (await enumerator.MoveNextAsync(ct))
                {
                    result.Add(enumerator.Current);
                }

                return this.Json(result);
            }
        }


        // PUT api/values/staff
        [HttpPut("{staff}")]
        public async Task<IActionResult> Put(string staff)
        {
            IReliableDictionary<string, int> StaffDictionary = await this.stateManager.GetOrAddAsync<IReliableDictionary<string, int>>("counts");

            using (ITransaction tx = this.stateManager.CreateTransaction())
            {
                await StaffDictionary.SetAsync(tx, staff, 1);
                await tx.CommitAsync();
            }
            telemetry.TrackEvent($"Added a Staff for {staff}");
            return new OkResult();
        }

        //DELETE api/values/5
        [HttpDelete("{staff}")]
        public async Task<IActionResult> Delete(string staff)
        {

            IReliableDictionary<string, int> StaffDictionary = await this.stateManager.GetOrAddAsync<IReliableDictionary<string, int>>("counts");

            using (ITransaction tx = this.stateManager.CreateTransaction())
            {
                await StaffDictionary.TryRemoveAsync(tx, staff);
                await tx.CommitAsync();

                telemetry.TrackEvent($"Removed a Staff for {staff}");
            }
            return new OkResult() ;
        }
    }
}
