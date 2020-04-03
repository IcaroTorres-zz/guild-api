﻿using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Newtonsoft.Json;

namespace Application.Hateoas.DTOs
{
  public class PaginationResourceDTO<T> : ResourceDTO where T : ResourceDTO
  {
    public PaginationResourceDTO(Pagination<T> valores) : base(valores.Items)
    {
      Page = valores.Page;
      PageSize = valores.PageSize;
      Pages = valores.Pages;
      Count = valores.Count;
      DataList = valores.Items.ToList();
    }
    private List<T> DataList { get; }

    [JsonProperty("items", Order = 2)] public override object Data => DataList;
    [JsonProperty("inPage", Order = -1)] public long InPage => DataList.Count;
    [JsonProperty("page", Order = -1)] public long Page { get; private set; }
    [JsonProperty("pageSize", Order = -1)] public long PageSize { get; private set; }
    [JsonProperty("pages", Order = -1)] public long Pages { get; private set; }
    [JsonProperty("count", Order = -1)] public long Count { get; private set; }
  }
}
