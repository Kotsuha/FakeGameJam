using System;

public interface IItem
{
    event Action<string> OnGetItem;
    string Name { get; }
    void SetEffect(bool isShow);
}


public interface IBuffItem : IItem, IBuff
{
    float GetScore();
}


public interface IDebuffItem : IItem, IDebuff
{

}




public interface IDebuff
{
    void DeBuff();
}

public interface IBuff
{
    void Buff();
}