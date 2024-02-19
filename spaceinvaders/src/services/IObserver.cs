using spaceinvaders.model.barricades;

namespace spaceinvaders.services;

public interface IObserver
{
    void Notify(BarricadeBlockPart blockPart);
}