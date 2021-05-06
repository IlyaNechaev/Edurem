
try:
    from calc import add as test_method
except Exception as e:
    print("[ERROR]" + str(e))

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
            try:
                self.currentResult["tests"] += 1
                if test_method(param["input"][0], param["input"][1]) == param["output"]:
                    self.currentResult["success"] += 1
                else:
                    self.currentResult["failures"] += 1
            except Exception as e:
                self.currentResult["error"] = e
                return

test = MyTest()
params = [ { "input": [1, 2], "output": 3 }, { "input": [1, -1], "output": 0 } ]

test.run_tests(params)
test.printResults()
