using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Core.Data
{
    public class SqlManager
    {
        public static List<Dictionary<string, object>> ExecuteQuery(string sql, Dictionary<string, object> parameters = null, CustomConnection connection = null)
        {
            var list = new List<Dictionary<string, object>>();

            IDataReader dataReader = null;
            IDbCommand command = null;

            connection = connection ?? new OracleConnection();
           
            try
            {
                connection.Connect();

                command = connection.CreateCommand();

                command.CommandText = sql;

                if(parameters!=null)
                    command.AddParameters(parameters);

                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    var dictionary = new Dictionary<string, object>();

                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        dictionary.Add(dataReader.GetName(i), dataReader.GetValue(i));
                    }

                    list.Add(dictionary);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dataReader != null)
                    dataReader.Close();

                connection.Disconnect();
            }

            return list;
        }

        public static object ExecuteScalar(string sql, Dictionary<string, object> parameters = null, CustomConnection connection = null)
        {
            object result = null;

            connection = connection ?? new OracleConnection();

            IDbCommand command = null;

            try
            {
                connection.Connect();

                command = connection.CreateCommand();

                command.CommandText = sql;

                if (parameters != null)
                    command.AddParameters(parameters);

                result = command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                connection.Disconnect();
            }

            return result;
        }

        public static int ExecuteNonQuery(string sql, Dictionary<string, object> parameters = null, CustomConnection connection = null)
        {
            int affectedRowsCount;

            connection = connection ?? new OracleConnection();

            IDbCommand command = null;

            try
            {
                connection.Connect();

                command = connection.CreateCommand();

                command.CommandText = sql;

                if (parameters != null)
                    command.AddParameters(parameters);

                affectedRowsCount = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                connection.Disconnect();
            }

            return affectedRowsCount;
        }

        public static bool Any(string sql, Dictionary<string, object> parameters = null, CustomConnection connection = null)
        {
            sql = string.Format("select count(0) from ({0}) t", sql);

            var result = Convert.ToInt32(ExecuteScalar(sql, parameters, connection));

            return result != 0;
        }
    }

    public enum EqualityFilterType
    {
        Equal,
        LessThanOrEqual,
        GreaterThanOrEqual,
        LessThan,
        GreaterThan,
        StartsWith,
        EndsWith,
        Contains
    }

    //public class EqualityFilter<T> where T : PersistentObject
    //{
    //    public string ColumnName { get; set; }

    //    public EqualityFilterType? FilterType { get; set; }

    //    public object Value { get; set; }

    //    string Expression { get; set; }

    //    public EqualityFilter(Property property, EqualityFilterType filterType, object value)
    //    {
    //        ColumnName = property.Column.Name;

    //        FilterType = filterType;

    //        Value = value;
    //    }

    //    public EqualityFilter(Expression<Func<T, object>> property, EqualityFilterType filterType, object value)
    //    {
    //        if (property.Body is MemberExpression)
    //        {
    //            var memberInfo = (property.Body as MemberExpression).Member;

    //            var table = Table.Get(typeof(T));

    //            ColumnName = table.ClassInfo.FindPropertyByName(memberInfo.Name).Column.Name;

    //            FilterType = filterType;

    //            Value = value;
    //        }
    //    }

    //    public EqualityFilter(Expression<Func<T, object>> filter)
    //    {
    //        var binaryExpression = ((UnaryExpression)filter.Body).Operand as BinaryExpression;

    //        if (binaryExpression!=null)
    //        {
    //            var propertyName = (binaryExpression.Left as MemberExpression).Member.Name;

    //            var table = Table.Get(typeof(T));

    //            ColumnName = table.ClassInfo.FindPropertyByName(propertyName).Column.Name;

    //            Value = System.Linq.Expressions.Expression.Lambda(binaryExpression.Right)
    //                        .Compile()
    //                        .DynamicInvoke();

    //            FilterType = (EqualityFilterType)Enum.Parse(typeof(EqualityFilterType), binaryExpression.NodeType.ToString());
    //        }
    //    }

      
    //    public string GetExpression(string alias)
    //    {
    //        switch (FilterType)
    //        {
    //            case EqualityFilterType.Equal:
    //                Expression = string.Format("{0}.{1}='{2}'", alias, ColumnName, Value);
    //                break;

    //            case EqualityFilterType.LessThanOrEqual:
    //                Expression = string.Format("{0}.{1}<='{2}'", alias, ColumnName, Value);
    //                break;

    //            case EqualityFilterType.GreaterThanOrEqual:
    //                Expression = string.Format("{0}.{1}>='{2}'", alias, ColumnName, Value);
    //                break;

    //            case EqualityFilterType.LessThan:
    //                Expression = string.Format("{0}.{1}<'{2}'", alias, ColumnName, Value);
    //                break;

    //            case EqualityFilterType.GreaterThan:
    //                Expression = string.Format("{0}.{1}>'{2}'", alias, ColumnName, Value);
    //                break;

    //            case EqualityFilterType.StartsWith:
    //                Expression = string.Format("{0}.{1} like '{2}%'",alias,ColumnName,Value);
    //                break;

    //            case EqualityFilterType.EndsWith:
    //                Expression = string.Format("{0}.{1} like '%{2}'", alias, ColumnName, Value);
    //                break;

    //            case EqualityFilterType.Contains:
    //                Expression = string.Format("{0}.{1} like '%{2}%'", alias, ColumnName, Value);
    //                break;
    //        }

    //        return Expression;
    //    }
    //}

    //public enum JointType
    //{
    //    InnerJoin,
    //    LeftJoin,
    //    RightJoin
    //}


    //public class JointPoint<T1,T2>
    //    where T1 : PersistentObject
    //    where T2 : PersistentObject
    //{
    //    public string LeftTableName { get; set; }

    //    public string RightTableName { get; set; }

    //    public string LeftKey { get; set; }

    //    public string RightKey { get; set; }

    //    public string LeftAlias { get; set; }

    //    public string RightAlias { get; set; }

    //    public JointType JointType { get; set; }

    //    public List<string> RightSelectableColumns { get; set; }

    //    public JointPoint() { }

    //    public JointPoint(Expression<Func<T1, object>> leftKey,
    //        Expression<Func<T2, object>> rightKey,
    //        JointType jointType, 
    //        string leftAlias = null,
    //        string rightAlias = null,
    //        params Expression<Func<T2,object>>[] rightSelectedColumns)
    //    {
    //        var leftTable = Table.Get(typeof(T1));
    //        var rightTable = Table.Get(typeof(T2));

    //        LeftTableName = Table.Get(typeof(T1)).Name;
    //        RightTableName = Table.Get(typeof(T2)).Name;

    //        PropertyInfo leftPropertyInfo;
    //        PropertyInfo rightPropertyInfo;

    //        if (leftKey.Body is MemberExpression)
    //        {
    //            leftPropertyInfo = (leftKey.Body as MemberExpression).Member as PropertyInfo;
    //        }
    //        else
    //        {
    //            leftPropertyInfo = (((UnaryExpression)leftKey.Body).Operand as MemberExpression).Member as PropertyInfo;
    //        }

    //        if (rightKey.Body is MemberExpression)
    //        {
    //            rightPropertyInfo = (rightKey.Body as MemberExpression).Member as PropertyInfo;
    //        }
    //        else
    //        {
    //            rightPropertyInfo = (((UnaryExpression)rightKey.Body).Operand as MemberExpression).Member as PropertyInfo;
    //        }

    //        LeftKey = leftTable.ClassInfo.FindPropertyByName(leftPropertyInfo.Name).Column.Name;
    //        RightKey = rightTable.ClassInfo.FindPropertyByName(rightPropertyInfo.Name).Column.Name;

    //        LeftAlias = leftAlias;
    //        RightAlias = rightAlias;

    //        JointType = jointType;

    //        foreach (var item in rightSelectedColumns)
    //        {
    //            PropertyInfo propertyInfo = null;

    //            if (item.Body is MemberExpression)
    //            {
    //                propertyInfo = (item.Body as MemberExpression).Member as PropertyInfo;
    //            }
    //            else
    //            {
    //                propertyInfo = (((UnaryExpression)item.Body).Operand as MemberExpression).Member as PropertyInfo;
    //            }

    //            var columnName = rightTable.ClassInfo.FindPropertyByName(propertyInfo.Name).Column.Name;

    //            if (RightSelectableColumns == null)
    //                RightSelectableColumns = new List<string>();

    //            RightSelectableColumns.Add(columnName);
    //        }
    //    }

    //    private string Expression
    //    {
    //        get
    //        {
    //            return string.Format("{0}={1}", string.Format("{0}.{1}", LeftAlias, LeftKey), string.Format("{0}.{1}", RightAlias, RightKey));
    //        }
    //    }

    //    public string PreSql
    //    {
    //        get
    //        {
    //            return string.Join(",", RightSelectableColumns.Select(c => string.Format("{0}.{1}", RightAlias, c)));
    //        }
    //        //return string.Format("select {0}.*,{1}.* from({2}){0} {3} {4} {1} on {5}", LeftAlias, RightAlias, rawSqlString, JointTypeToString, RightTableName, Expression);
    //    }

    //    public string MidSql
    //    {
    //        get
    //        {
    //            return string.Format("{0} {1} {2} on {3} ", JointTypeToString, RightTableName, RightAlias, Expression);
    //        }
    //    }

    //    public string PostSql
    //    {
    //        get
    //        {
    //            return "";
    //        }
    //    }

    //    public string JointTypeToString
    //    {
    //        get
    //        {
    //            switch (JointType)
    //            {
    //                case JointType.InnerJoin:
    //                    return "inner join";

    //                case JointType.LeftJoin:
    //                    return "left join";

    //                case JointType.RightJoin:
    //                    return "right join";

    //                default:
    //                    return "";
    //            }
    //        }
    //    }
    //}


    //public class SqlGenerator
    //{
    //    public SqlGenerator(TableType tableType = Data.TableType.Oracle)
    //    {
    //        TableType = tableType;

    //        Joints = new List<JointPoint<PersistentObject, PersistentObject>>();
    //    }

    //    public TableType TableType { get; set; }

    //    Table Table { get; set; }

    //    public string RawSqlString { get; set; }

    //    public string FilteredSqlString { get; set; }

    //    public string PagedSqlString { get; set; }

    //    public int PageIndex { get; set; }

    //    public int PageSize { get; set; }

    //    public int TotalRecordCount { get; set; }

    //    public int FilteredRecordCount { get; set; }

    //    public int AliasCounter { get; set; }

    //    public List<JointPoint<PersistentObject, PersistentObject>> Joints { get; set; }

    //    private string CalculateAlias()
    //    {
    //        return string.Format("t{0}", AliasCounter++);
    //    }

    //    public SqlGenerator AddJointPoint<T1, T2>(params JointPoint<T1, T2>[] joints)
    //        where T1 : PersistentObject
    //        where T2 : PersistentObject
    //    {
    //        if (Joints == null)
    //            Joints = new List<JointPoint<PersistentObject, PersistentObject>>();

    //        foreach (var j in joints)
    //        {
    //            var joint = new JointPoint<PersistentObject, PersistentObject>()
    //            {
    //                LeftTableName = j.LeftTableName,
    //                RightTableName = j.RightTableName,
    //                LeftAlias = j.LeftAlias,
    //                RightAlias = j.RightAlias,
    //                LeftKey = j.LeftKey,
    //                RightKey = j.RightKey,
    //                JointType = j.JointType,
    //                RightSelectableColumns = j.RightSelectableColumns
    //            };

    //            Joints.Add(joint);
    //        }

    //        return this;
    //    }

    //    public SqlGenerator Select(Table table)
    //    {
    //        Table = table;

    //        var alias0 = CalculateAlias();

    //        if(Joints.Any())
    //        {
    //            var postString = "";

    //            foreach (var joint in Joints)
    //            {
    //                joint.LeftAlias = alias0;

    //                var alias1 = CalculateAlias();

    //                joint.RightAlias = alias1;

    //                //select alias0.*,alias1.Name from Table1 alias0 inner join Table2 alias1 on alias0.Id=alias1.Id

    //                postString += joint.MidSql;

    //                //RawSqlString = string.Format("select {0},{3} from {1} {2} {4} {5} {6}",
    //                //    table.Columns.Any(c => c.IsSelectable) ? string.Join(",", table.Columns.Where(c => c.IsSelectable).Select(c => string.Format("{0}.{1}", alias0, c.Name))) : string.Format("{0}.*", alias0),
    //                //    table.Name,
    //                //    alias0);
    //            }

    //            var preString = string.Format("select {0},{1} from {2} {3} ",
    //                table.Columns.Any(c => c.IsSelectable) ? string.Join(",", table.Columns.Where(c => c.IsSelectable).Select(c => string.Format("{0}.{1}", alias0, c.Name))) : string.Format("{0}.*", alias0),
    //                string.Join(",", Joints.Select(j => j.PreSql)),
    //                table.Name,
    //                alias0);

    //            RawSqlString = preString + postString;
    //        }
    //        else
    //        {
    //            RawSqlString = string.Format("select {0} from {1} {2}",
    //                table.Columns.Any(c => c.IsSelectable) ? string.Join(",", table.Columns.Where(c => c.IsSelectable).Select(c => string.Format("{0}.{1}", alias0, c.Name))) : string.Format("{0}.*", alias0),
    //                table.Name,
    //                alias0);
    //        }

    //        return this;
    //    }

    //    //public SqlGenerator Select(string tableName)
    //    //{
    //    //    Table = Table.Get(Type.GetType(tableName));

    //    //    var alias0 = CalculateAlias();

    //    //    RawSqlString = string.Format("select {1}.* from {0} {1}", tableName, alias0);

    //    //    return this;
    //    //}

    //    public SqlGenerator Insert(Table table)
    //    {
    //        Table = table;

    //        RawSqlString = string.Format("insert into {0} ({1}) values ({2})", table.Name, string.Join(",", table.Columns.Select(c => c.Name)), string.Join(",", table.Columns.Select(c => string.Format("@{0}", c.Name))));

    //        return this;
    //    }


    //    public SqlGenerator Filter(params Column[] filters)
    //    {
    //        if (filters != null && filters.Any())
    //        {
    //            var alias0 = CalculateAlias();

    //            FilteredSqlString = string.Format("select {2}.* from ({0}){2}  where {1}", RawSqlString, string.Join(" and ", filters.Select(f => string.Format("{0}.{1}='{2}'",alias0 ,f.Name, f.Value))),alias0);
    //        }

    //        return this;
    //    }

    //    public SqlGenerator Filter(params Property[] filters)
    //    {
    //        var columnList = filters.Select(f=>f.Column);

    //        if (columnList != null && columnList.Any())
    //        {
    //            var alias0 = CalculateAlias();

    //            FilteredSqlString = string.Format("select {2}.* from ({0}){2}  where {1}", RawSqlString, string.Join(" and ", columnList.Select(c => string.Format("{0}.{1}='{2}'", alias0, c.Name, c.Value))), alias0);
    //        }

    //        return this;
    //    }

    //    public SqlGenerator Filter<T>(params EqualityFilter<T>[] filters) where T : PersistentObject
    //    {
    //        if (filters != null && filters.Any())
    //        {
    //            var alias0 = CalculateAlias();

    //            FilteredSqlString = string.Format("select {2}.* from ({0}){2}  where {1}", RawSqlString, string.Join(" and ", filters.Select(f => f.GetExpression(alias0))),alias0);
    //        }

    //        return this;
    //    }

    //    public SqlGenerator FullSearch(string textToSearch)
    //    {
    //        var columnsToSearch = Table.GetAllSearchableColumn()
    //                                    .ToArray();

    //        if (!string.IsNullOrWhiteSpace(textToSearch) && columnsToSearch != null && columnsToSearch.Any(c => !string.IsNullOrEmpty(c.Name)))
    //        {
    //            var alias0 = CalculateAlias();

    //            FilteredSqlString = string.Format("select {2}.* from ({0}){2}  where {1}",
    //                                    RawSqlString,
    //                                    string.Join(" or ", columnsToSearch.Select(c => string.Format("UPPER({0}.{1}) like '%{2}%'", alias0, c.Name, textToSearch.ToUpper(new CultureInfo("en-US", false))))),
    //                                    alias0);
    //        } 

    //        return this;
    //    }

    //    public SqlGenerator OrderBy()
    //    {
    //        var orderedColumns = Table.GetAllSortableColumn()
    //                                    .Where(c => c.IsSorting)
    //                                    .ToArray();

    //        if (orderedColumns != null && orderedColumns.Any(c=>!string.IsNullOrEmpty(c.Name)))
    //        {
    //            var alias0 = CalculateAlias();
    //            FilteredSqlString = string.Format("select {2}.* from ({0}){2} order by {1}", FilteredSqlString ?? RawSqlString, string.Join(",", orderedColumns.Select(c => string.Format("{0}.{1} {2}", alias0, c.Name, c.SortDirection == SortDirection.Ascending ? "asc" : "desc"))), alias0);
    //        }

    //        return this;
    //    }

    //    public SqlGenerator Take(int pageIndex = 0, int pageSize = 10)
    //    {
    //        var alias0 = CalculateAlias();

    //        PageIndex = pageIndex;
    //        PageSize = pageSize;

    //        var orderedColumns = Table.GetAllSortableColumn()
    //            .Where(c => c.IsSorting)
    //            .ToArray();

    //        var orderedQuery = string.Join(",", orderedColumns.Select(c => string.Format("{0}.{1} {2}", alias0, c.Name, c.SortDirection == SortDirection.Ascending ? "asc" : "desc")));

    //        if (TableType == Data.TableType.Oracle)
    //        {
    //            var alias1 = CalculateAlias();

    //            PagedSqlString = string.Format("SELECT {4}.* FROM ( SELECT rownum r__,{3}.* FROM({0}) {3} WHERE rownum < (({1} * {2}) + 1 )) {4} WHERE r__ >= ((({1}-1) * {2}) + 1)",
    //                orderedQuery,
    //                PageIndex,
    //                PageSize,
    //                alias0,
    //                alias1);
    //        }
    //        else
    //        {
    //            var sql = FilteredSqlString ?? RawSqlString;

    //            var startIndex = sql.IndexOf("order by");

    //            if (startIndex > 0)
    //                sql = sql.Remove(startIndex);

    //            if (orderedColumns.Any())
    //            {
    //                PagedSqlString = string.Format("select {1}.* from ({0}){1} order by {2} OFFSET {3} ROWS FETCH NEXT {4} ROWS ONLY",
    //                    sql,
    //                    alias0,
    //                    orderedQuery,
    //                    PageIndex,
    //                    PageSize);
    //            }
    //            else
    //            {
    //                PagedSqlString = FilteredSqlString ?? RawSqlString;
    //            }
    //        }

    //        return this;
    //    }
    //}

    public static class SqlManagerExtensions
    {
        public static void AddParameters(this IDbCommand command, Dictionary<string, object> parameters)
        {
            foreach (var pair in parameters)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = pair.Key;
                parameter.Value = pair.Value ?? DBNull.Value;

                command.Parameters.Add(parameter);
            }
        }
    }
}
