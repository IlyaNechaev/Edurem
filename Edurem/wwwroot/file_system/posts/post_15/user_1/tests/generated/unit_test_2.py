try:
    from calc import sub as test_method
except Exception as e:
    print("[ERROR]" + str(e))
    print("[INFO]")

class MyTest():

    currentResult = { "tests": 0, "failures": 0, "success": 0, "error": "" } # holds last result object passed to run method

    def printResults(self):
        print("{")
        print("  \"tests\": " + str(self.currentResult["tests"]) + ",")
        print("  \"failures\": " + str(self.currentResult["failures"]) + ",")
        print("  \"success\": " + str(self.currentResult["success"]) + ",")
        print("  \"error\": \"" + str(self.currentResult["error"]) + "\"")
        print("}")

    def run_tests(self, params):
        for param in params:
            self.currentResult["tests"] += 1
            try:
                if test_method(param["input"][0], param["input"][1]) == param["output"]:
                    self.currentResult["success"] += 1
                else:
                    self.currentResult["failures"] += 1
            except Exception as e:
                if self.currentResult["error"] == "":
                    self.currentResult["error"] = e

test = MyTest()
params = [ { "input": [5, 1], "output": 4 }, { "input": [1, 2], "output": -1 } ]

test.run_tests(params)
test.printResults()
