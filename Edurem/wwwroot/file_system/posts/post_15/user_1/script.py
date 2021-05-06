print("%TEST%")
try:
    import unit_test_1
except Exception as e:
    print("[ERROR]" + "Exception: " + str(e))
print("%TEST%")
try:
    import unit_test_2
except Exception as e:
    print("[ERROR]" + "Exception: " + str(e))