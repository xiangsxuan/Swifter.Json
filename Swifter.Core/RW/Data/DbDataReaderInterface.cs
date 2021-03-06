﻿using Swifter.RW;

using System;
using System.Data;
using static Swifter.RW.DataTableRW;

namespace Swifter.RW
{
    sealed class DbDataReaderInterface<T> : IValueInterface<T> where T : class, System.Data.IDataReader
    {
        public T ReadValue(IValueReader valueReader)
        {
            if (valueReader is IValueReader<T> reader)
            {
                return reader.ReadValue();
            }

            if (typeof(T).IsAssignableFrom(typeof(DataTableReader)))
            {
                return Unsafe.As<T>(ValueInterface<DataTable>.ReadValue(valueReader)?.CreateDataReader());
            }

            throw new NotSupportedException();
        }

        public void WriteValue(IValueWriter valueWriter, T value)
        {
            var reader = new DbDataReaderReader(value, valueWriter is ITargetedBind targeted && targeted.TargetedId != 0 ? targeted.GetDataTableRWOptions() : DefaultOptions);

            valueWriter.WriteArray(reader);
        }
    }
}