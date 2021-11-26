#/bin/sh

docker exec -it nostromo-brett redis-cli
# docker exec -it nostromo-brett redis-cli XRANGE nostromo-brett - +

# XADD nostromo-brett * temperature 0.123 "sensor type" Ds18b20
# XRANGE nostromo-brett - +
