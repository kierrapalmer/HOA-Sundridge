﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HOASunridge.Pages.Shared {
    public class PaginatedList<T> : List<T> {
        public int PageIndex { get; }
        public int TotalPages { get; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize) {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage {
            get {
                return PageIndex > 1;
            }
        }

        public bool HasNextPage {
            get {
                return PageIndex < TotalPages;
            }
        }

        public static async Task<PaginatedList<T>> CreateAsync(
            IQueryable<T> source, int pageIndex, int pageSize) {
            var count = await source.CountAsync().ConfigureAwait(false);
            var items = await source.Skip(
                    (pageIndex - 1) * pageSize)
                .Take(pageSize).ToListAsync().ConfigureAwait(false);
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}