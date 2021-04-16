FROM python:3

COPY ..\calc_test.py .
COPY calc.py .

RUN pip install pystrich

CMD [ "python", "calc_test.py" ]