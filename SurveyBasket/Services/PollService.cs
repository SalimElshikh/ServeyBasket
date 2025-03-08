namespace SurveyBasket.Services;

public class PollService : IPollService
{
    private readonly  static List<Poll> _polls = [
        new Poll {
            Id = 1,
            Title = "First Poll",
            Description = "My First Poll In Project"
        },
        new Poll {
            Id = 2,
            Title = "Second Poll",
            Description = "My Second Poll In Project"
        }

    ];

    public Poll Add(Poll poll)
    {
        poll.Id = _polls.Count + 1; 
        _polls.Add( poll );
        return poll;
    }

    

    public IEnumerable<Poll> GetAll() =>  _polls;

    public Poll? GetById(int id) => _polls.FirstOrDefault(pol => pol.Id == id);

    public bool Update(int id,Poll poll )
    {
        var currentPoll  = GetById(id);

        if(currentPoll is null)
            return false; 

        currentPoll.Title = poll.Title;
        currentPoll.Description = poll.Description;

        return true;
    }
    public bool Delete(int id)
    {
        var poll = GetById(id);

        if (poll is null)
            return false;
        _polls.Remove(poll);
        return true;
    }

}
