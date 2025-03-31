#!/bin/bash
systemctl disable DA.DinnerPlanner.Web.service
systemctl start DA.DinnerPlanner.Web.service
systemctl status DA.DinnerPlanner.Web.service

systemctl disable DA.DinnerPlanner.Blazor.service
systemctl start DA.DinnerPlanner.Blazor.service
systemctl status DA.DinnerPlanner.Blazor.service
