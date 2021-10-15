﻿using Luban.Job.Cfg.Defs;
using Luban.Job.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Luban.Job.Cfg.DataSources.Excel
{
    class TitleRow
    {
        public List<string> Tags { get; }

        public Title SelfTitle { get; }

        public object Current
        {
            get
            {
                var v = Row[SelfTitle.FromIndex].Value;
                if (v == null || (v is string s && string.IsNullOrEmpty(s) && !string.IsNullOrEmpty(SelfTitle.Default)))
                {
                    return SelfTitle.Default;
                }
                else
                {
                    return v;
                }
            }
        }

        public List<Cell> Row { get; }

        public int CellCount => SelfTitle.ToIndex - SelfTitle.FromIndex + 1;

        public List<List<Cell>> Rows { get; }

        public Dictionary<string, TitleRow> Fields { get; }

        public List<TitleRow> Elements { get; }

        public ExcelStream AsStream(string sep)
        {
            if (string.IsNullOrEmpty(SelfTitle.Sep))
            {
                if (string.IsNullOrEmpty(sep))
                {
                    return new ExcelStream(Row, SelfTitle.FromIndex, SelfTitle.ToIndex, "", SelfTitle.Default);
                }
                else
                {
                    return new ExcelStream(Row, SelfTitle.FromIndex, SelfTitle.ToIndex, sep, SelfTitle.Default);
                }
            }
            else
            {
                //if (string.IsNullOrEmpty(sep) || sep == SelfTitle.Sep)
                //{
                //    return new ExcelStream(Row, SelfTitle.FromIndex, SelfTitle.ToIndex, sep);
                //}
                //else
                //{
                // SelfTitle.Sep 设置覆盖 bean的 sep设置（只有这个可能）
                return new ExcelStream(Row, SelfTitle.FromIndex, SelfTitle.ToIndex, SelfTitle.Sep, SelfTitle.Default);
                //}
            }
        }


        public bool HasSubFields => Fields != null || Elements != null;

        public TitleRow(Title selfTitle, List<Cell> row)
        {
            SelfTitle = selfTitle;
            Row = row;
        }

        public TitleRow(Title selfTitle, List<List<Cell>> rows)
        {
            SelfTitle = selfTitle;
            Rows = rows;
        }

        public TitleRow(Title selfTitle, Dictionary<string, TitleRow> fields)
        {
            SelfTitle = selfTitle;
            Fields = fields;
        }

        public TitleRow(Title selfTitle, List<TitleRow> elements)
        {
            SelfTitle = selfTitle;
            Elements = elements;
        }

        public int RowCount => Rows.Count;

        public Title GetTitle(string name)
        {
            return SelfTitle.SubTitles.TryGetValue(name, out var title) ? title : null;
        }


        public TitleRow GetSubTitleNamedRow(string name)
        {
            //Title title = Titles[name];
            //return new TitleRow(title, this.Rows);
            return Fields.TryGetValue(name, out var r) ? r : null;
        }

        public IEnumerable<ExcelStream> GetColumnOfMultiRows(string name, string sep)
        {
            foreach (var ele in GetSubTitleNamedRow(name).Elements)
            {
                yield return ele.AsStream(sep);
            }
        }


        public IEnumerable<ExcelStream> AsMultiRowStream(string sep)
        {
            //if (Titles.TryGetValue(name, out var title))
            //{
            //    if (isRowOrient)
            //    {
            //        var totalCells = Rows.SelectMany(r => r.GetRange(title.FromIndex, title.ToIndex - title.FromIndex + 1))
            //            .Where(c => c.Value != null && !(c.Value is string s && string.IsNullOrWhiteSpace(s))).ToList();
            //        return new ExcelStream(totalCells, 0, totalCells.Count - 1, sep, false);
            //    }
            //    else
            //    {
            //        throw new NotSupportedException($"bean类型多行数据不支持纵向填写");
            //    }
            //}
            //else
            //{
            //    throw new Exception($"单元薄 缺失 列:{name}，请检查是否写错或者遗漏");
            //}
            throw new NotSupportedException();
        }
    }
}