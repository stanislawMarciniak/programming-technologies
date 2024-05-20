using Presentation.Model.API;
using System;
using System.Windows.Input;

namespace Presentation.ViewModel;

public interface IEventDetailViewModel
{
    static IEventDetailViewModel CreateViewModel(int id, int stateId, int userId, DateTime occurrenceDate, 
        string type, int? quantity, IEventModelOperation model, IErrorInformer informer)
    {
        return new EventDetailViewModel(id, stateId, userId, occurrenceDate, type, quantity, model, informer);
    }

    ICommand UpdateEvent { get; set; }

    int Id { get; set; }

    int StateId { get; set; }

    int UserId { get; set; }

    DateTime OccurrenceDate { get; set; }

    string Type { get; set; }

    int? Quantity { get; set; }
}
