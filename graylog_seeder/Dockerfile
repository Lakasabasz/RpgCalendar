FROM python:3.8-slim-buster

COPY graylog_seeder/graylog_seeder.py /graylog_seeder.py
WORKDIR /
RUN [ "pip3", "install", "requests"] 
CMD [ "python", "graylog_seeder.py"] 