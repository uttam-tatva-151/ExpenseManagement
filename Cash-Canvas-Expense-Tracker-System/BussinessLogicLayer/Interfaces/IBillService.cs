using CashCanvas.Core.Beans;
using CashCanvas.Core.ViewModel;

namespace CashCanvas.Services.Interfaces;

public interface IBillService
{
    Task<List<BillViewModel>> GetAllBillsAsync(Guid userId,PaginationDetails pagination);
    Task<BillViewModel?> GetBillByIdAsync( Guid billId);

    Task<ResponseResult<int>> CreateBillAsync(BillViewModel billViewModel);
    Task<ResponseResult<bool>> UpdateBillAsync(BillViewModel billViewModel);
    Task<ResponseResult<bool>> MoveToTrashAsync( Guid billId);
}
