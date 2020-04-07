using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Newtonsoft.Json;

namespace Application.Hateoas.DTOs
{
	public class PaginationResourceDto<T> : ResourceDto where T : ResourceDto
	{
		public PaginationResourceDto(Pagination<T> values) : base(values.Data)
		{
			Page = values.Page;
			PageSize = values.PageSize;
			Pages = values.Pages;
			Count = values.Count;
			DataList = values.Data.ToList();
		}

		private List<T> DataList { get; }

		/* [JsonProperty("data", Order = 2)]*/ public override object Data => DataList;
		[JsonProperty("inPage", Order = -1)] public long InPage => DataList.Count;
		[JsonProperty("page", Order = -1)] public long Page { get; private set; }
		[JsonProperty("pageSize", Order = -1)] public long PageSize { get; private set; }
		[JsonProperty("pages", Order = -1)] public long Pages { get; private set; }
		[JsonProperty("count", Order = -1)] public long Count { get; private set; }
	}
}