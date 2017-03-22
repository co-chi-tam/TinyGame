
namespace FSM
{
    public interface IState
    {
        void StartState();
        void UpdateState();
        void ExitState();
    }
}
