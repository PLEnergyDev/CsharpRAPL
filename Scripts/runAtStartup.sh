#!/bin/bash

echo 0 | sudo tee /sys/devices/system/cpu/cpu6/online
echo 0 | sudo tee /sys/devices/system/cpu/cpu7/online
echo 0 | sudo tee /sys/devices/system/cpu/cpu8/online
echo 0 | sudo tee /sys/devices/system/cpu/cpu9/online
echo 0 | sudo tee /sys/devices/system/cpu/cpu10/online
echo 0 | sudo tee /sys/devices/system/cpu/cpu11/online

sudo sysctl vm.swappiness=10
sudo sysctl kernel.randomize_va_space=0
sudo sysctl kernel.perf_cpu_time_max_percent=1

echo "1" | sudo tee /sys/devices/system/cpu/intel_pstate/no_turbo
