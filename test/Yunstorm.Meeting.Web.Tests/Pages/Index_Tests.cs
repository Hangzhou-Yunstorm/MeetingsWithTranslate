using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Yunstorm.Meeting.Pages
{
    public class Index_Tests : MeetingWebTestBase
    {
        [Fact]
        public async Task Welcome_Page()
        {
            var response = await GetResponseAsStringAsync("/");
            response.ShouldNotBeNull();
        }
    }
}
