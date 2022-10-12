namespace SmallCat.FreeSqlIdleBus;

public interface IDataEntity { }
public interface IDataEntity<T> : IDataEntity where T : IDbLocker { }
public interface IDataEntity<T1, T2> : IDataEntity where T1 : IDbLocker where T2 : IDbLocker { }
public interface IDataEntity<T1, T2, T3> : IDataEntity where T1 : IDbLocker where T2 : IDbLocker where T3 : IDbLocker { }
// 可以根据业务需求，自行拓展;
