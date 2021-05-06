FROM python:3

COPY "./user_1/tests/generated/unit_test_1.py" "."
COPY "./user_1/tests/generated/unit_test_2.py" "."
COPY "./user_1/script.py" "."
COPY "./user_1/solution/calc.py" "."

CMD [ "python", "script.py" ]
