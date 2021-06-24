print("%TEST%")
try:
    import unit_test_1
except Exception as e:
    print("[ERROR]" + "Exception: " + str(e))