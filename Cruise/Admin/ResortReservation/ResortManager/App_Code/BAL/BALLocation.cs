﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BALLocation
/// </summary>
public class BALLocation
{
	public BALLocation()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string LocationName { get; set; }
    public int LocationId { get; set; }
    public string action { get; set; }
    public int countryid { get; set; }
    public string countryname { get; set; }

    public string Description { get; set; }


}