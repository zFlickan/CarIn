﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarIn.DAL.Repositories.Abstract;
using CarIn.Models.Entities;
using CarIn.Models.ViewModels;

namespace CarIn.BLL
{
    public class ProccessReqFromWebService
    {
        private readonly IRepository<TrafficIncident> _trafficRepository;
        private readonly IRepository<WheatherPeriod> _wheaterRepository;
        private readonly IRepository<VasttrafikIncident> _vasttrafikRepository;


        public ProccessReqFromWebService(IRepository<TrafficIncident> trafficRepository, IRepository<WheatherPeriod> wheaterRepository, IRepository<VasttrafikIncident> vasttrafikRepository)
        {
            _trafficRepository = trafficRepository;
            _wheaterRepository = wheaterRepository;
            _vasttrafikRepository = vasttrafikRepository;
        }

        public MapInfoVm ProccesReqFromParams(string traffic, string wheather, string localTraffic)
        {
            try
            {
                var mapInfoModel = new MapInfoVm();

                if (string.IsNullOrWhiteSpace(traffic))
                {
                    mapInfoModel.TrafficIncidents = null;
                }
                else
                {
                    switch (traffic.ToLower())
                    {
                        case "all":
                            mapInfoModel.TrafficIncidents = _trafficRepository.FindAll().ToList();
                            break;
                        case "serious":
                            mapInfoModel.TrafficIncidents = _trafficRepository.FindAll(x => int.Parse(x.Severity) == 4).ToList();
                            break;
                        default:
                            mapInfoModel.TrafficIncidents = null;
                            break;
                    }
                }


                if (string.IsNullOrWhiteSpace(wheather))
                {
                    mapInfoModel.WheatherPeriods = null;
                }
                else
                {
                    mapInfoModel.WheatherPeriods = wheather.ToLower() == "all" ? _wheaterRepository.FindAll().ToList() : null;
                }


                if (string.IsNullOrWhiteSpace(localTraffic))
                {
                    mapInfoModel.VasttrafikIncidents = null;
                }
                else
                {
                    if (localTraffic.ToLower() == "all")
                    {
                        mapInfoModel.VasttrafikIncidents = _vasttrafikRepository.FindAll().ToList();
                    }
                    else if (localTraffic.ToLower() == "serious")
                    {
                        mapInfoModel.VasttrafikIncidents = _vasttrafikRepository.FindAll(x => int.Parse(x.Priority) == 1).ToList();
                    }
                    else
                    {
                        mapInfoModel.VasttrafikIncidents = null;
                    }
                }
                return mapInfoModel;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public MapInfoVm ProccesReqGetAll()
        {
            try
            {
                var mapInfoModel = new MapInfoVm
                {
                    TrafficIncidents = _trafficRepository.FindAll().ToList(),
                    WheatherPeriods = _wheaterRepository.FindAll().ToList(),
                    VasttrafikIncidents = _vasttrafikRepository.FindAll().ToList()
                };
                return mapInfoModel;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}