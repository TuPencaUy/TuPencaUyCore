using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.Core.DTOs;
using TuPencaUy.Site.DAO.Models;

namespace TuPencaUy.Core.DataServices.Services.Platform
{
  public class PlatformAnalyticsService : IAnalyticsService
  {
    private int _page = 1;
    private int _pageSize = 10;

    public List<BetUserDTO> GetLeaderboard(out int count, int eventId, int? page = null, int? pageSize = null)
    {
      SetPagination(page, pageSize);
      throw new NotImplementedException();
    }

    private void SetPagination(int? page, int? pageSize)
    {
      _page = page != null && page.Value > 0 ? page.Value : _page;
      _pageSize = pageSize != null && pageSize.Value > 0 ? pageSize.Value : _pageSize;
    }
  }
}
