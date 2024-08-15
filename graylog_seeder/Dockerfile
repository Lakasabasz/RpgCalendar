FROM python:3.8-slim-buster

COPY graylog_seeder.py /graylog_seeder.py
WORKDIR /
RUN [ "pip", "install", "requests"] 
CMD [ "python", "graylog_seeder.py"] 