#!/bin/bash
systemctl disable DA.DinnerPlanner.Web.service
systemctl start DA.DinnerPlanner.Web.service
systemctl status DA.DinnerPlanner.Web.service
